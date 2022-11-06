package content.common.innerCommunicators;

import content.common.innerCommunicators.errors.InnerCommunicationException;

import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.time.Duration;
import java.util.Objects;

import static content.common.utils.EnvironmentUtils.getApplicationContext;
import static content.common.utils.JwtUtils.getRequestJwtToken;
import static content.common.utils.StringUtils.concatenate;
import static java.net.http.HttpClient.newHttpClient;
import static java.time.temporal.ChronoUnit.SECONDS;
import static org.springframework.http.HttpHeaders.AUTHORIZATION;

public abstract class AbstractHttpCommunicator implements Communicator {

    private static final String PROTOCOL = "http://";
    private static final String LOCAL_INFIX = "localhost:8079/api/";
    private static final String APPLICATION_LOCAL = "local";
    private static final int REQUEST_DURATION_LIMIT = 3;

    private final HttpClient client;


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

            throw new InnerCommunicationException();
        }
    }

    private URI prepareUri(String path) throws URISyntaxException {

        return Objects.equals(getApplicationContext(), APPLICATION_LOCAL) ?
                new URI(concatenate(PROTOCOL, LOCAL_INFIX, path)) :
                new URI(concatenate(PROTOCOL, path));
    }

    private HttpRequest prepareRequest(URI uri) {

        return HttpRequest.newBuilder()
                .uri(uri)
                .header(AUTHORIZATION, getRequestJwtToken())
                .timeout(Duration.of(REQUEST_DURATION_LIMIT, SECONDS))
                .GET()
                .build();
    }
}
