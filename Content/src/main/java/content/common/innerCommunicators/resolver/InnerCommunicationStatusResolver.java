package content.common.innerCommunicators.resolver;

import org.springframework.http.HttpStatus;

import static org.springframework.http.HttpStatus.*;

public class InnerCommunicationStatusResolver {

    public static ResolvedStatus resolveIdCheckStatus(HttpStatus status, String entity) {

        if (status == OK) return new ResolvedStatus(OK, "");
        else if (status == NOT_FOUND) return new ResolvedStatus(NOT_FOUND, entity + " with given ID does not exist");
        else if (status == BAD_GATEWAY) return new ResolvedStatus(NOT_FOUND,"Communication between services has failed");
        else if (status == INTERNAL_SERVER_ERROR) return new ResolvedStatus(INTERNAL_SERVER_ERROR, "Other service has failed");
        else return new ResolvedStatus(status, status.getReasonPhrase());
    }
}
