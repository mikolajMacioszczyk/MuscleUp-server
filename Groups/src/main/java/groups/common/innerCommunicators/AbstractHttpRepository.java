package groups.common.innerCommunicators;

import groups.common.innerCommunicators.errors.InnerCommunicationException;
import groups.common.innerCommunicators.errors.NoAuthHeaderException;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import java.net.http.HttpResponse;

import static org.springframework.http.HttpStatus.INTERNAL_SERVER_ERROR;
import static org.springframework.http.HttpStatus.UNAUTHORIZED;

@Service
public abstract class AbstractHttpRepository extends AbstractHttpCommunicator {


    public HttpStatus checkIfIdExist(String path) {

        try {

            HttpResponse<String> response = sendInnerGetRequest(path);

            return HttpStatus.valueOf(response.statusCode());
        }
        catch(InnerCommunicationException e) {

            return INTERNAL_SERVER_ERROR;
        }
        catch(NoAuthHeaderException e) {

            return UNAUTHORIZED;
        }
    }

    public String getById(String path) {

        HttpResponse<String> response = sendInnerGetRequest(path);

        return response.body();
    }
}
