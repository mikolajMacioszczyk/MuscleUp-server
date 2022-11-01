package groups.common.innerCommunicators;

import groups.common.innerCommunicators.errors.InnerCommunicationException;
import groups.common.innerCommunicators.errors.NoAuthHeaderException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import javax.servlet.http.HttpServletRequest;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;
import java.util.Objects;

import static groups.common.utils.EnvironmentUtils.getApplicationContext;
import static groups.common.utils.StringUtils.concatenate;
import static java.net.http.HttpClient.newHttpClient;
import static java.time.temporal.ChronoUnit.SECONDS;
import static java.util.Objects.isNull;
import static org.springframework.http.HttpHeaders.AUTHORIZATION;

@Service
public abstract class AbstractHttpCommunicator implements Communicator {

    private static final String PROTOCOL = "http://";
    private static final String LOCAL_INFIX = "localhost:8079/api/";
    private static final String APPLICATION_LOCAL = "local";
    private static final String BEARER_PREFIX = "Bearer ";
    private static final int REQUEST_DURATION_LIMIT = 3;

    private final HttpClient client;


    @Autowired
    public AbstractHttpCommunicator() {

        this.client = newHttpClient();
    }


    @Override
    public HttpResponse<String> sendInnerGetRequest(String path) {

        try {

            URI uri = prepareUri(path);
            HttpRequest request = prepareRequest(uri);

            return client.send(request, HttpResponse.BodyHandlers.ofString());
        }
        catch(URISyntaxException | IOException | InterruptedException exception) {

            throw new InnerCommunicationException(
                    concatenate("Connection to other service failed\nPath: ", path),
                    exception
            );
        }
    }

    private URI prepareUri(String path) throws URISyntaxException {

        return Objects.equals(getApplicationContext(), APPLICATION_LOCAL) ?
                new URI(concatenate(PROTOCOL, LOCAL_INFIX, path)) :
                new URI(concatenate(PROTOCOL, path));
    }

    private HttpRequest prepareRequest(URI uri) {

        String jwtToken = concatenate(BEARER_PREFIX, getJwtToken());

        return HttpRequest.newBuilder()
                .uri(uri)
                .header(AUTHORIZATION, jwtToken)
                .timeout(Duration.of(REQUEST_DURATION_LIMIT, SECONDS))
                .GET()
                .build();
    }

    private String getJwtToken() {

        RequestAttributes attributes = RequestContextHolder.currentRequestAttributes();

        HttpServletRequest request = ((ServletRequestAttributes) attributes).getRequest();

        String jwtToken = request.getHeader(AUTHORIZATION);

        if (isNull(jwtToken)) throw new NoAuthHeaderException("Header with jwt Token not found");

        return jwtToken;
    }
}
