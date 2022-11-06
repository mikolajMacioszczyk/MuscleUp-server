package content.common.innerCommunicators;

import content.common.innerCommunicators.errors.InnerCommunicationException;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.net.http.HttpResponse;

import static org.springframework.http.HttpStatus.OK;


@Service
public abstract class AbstractHttpRepository extends AbstractHttpCommunicator {


    public HttpStatus checkIfIdExist(String path) {

        HttpResponse<String> response = sendInnerGetRequest(path);

        return HttpStatus.valueOf(response.statusCode());
    }

    public String getById(String path) {

        HttpResponse<String> response = sendInnerGetRequest(path);

        if (response.statusCode() != OK.value()) throw new InnerCommunicationException();

        return response.body();
    }
}
