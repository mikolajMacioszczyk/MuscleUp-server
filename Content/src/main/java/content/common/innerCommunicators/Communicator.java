package content.common.innerCommunicators;

import java.net.http.HttpResponse;

public interface Communicator {

    HttpResponse<String> sendInnerGetRequest(String path);
}
