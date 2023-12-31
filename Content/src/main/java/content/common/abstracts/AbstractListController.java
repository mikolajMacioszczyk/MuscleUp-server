package content.common.abstracts;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

public abstract class AbstractListController {

    protected static final String FITNESS_CLUB_HEADER = "FitnessClubId";

    protected ResponseEntity<?> response(HttpStatus responseStatus) {

        return new ResponseEntity<>(responseStatus);
    }

    protected ResponseEntity<?> response(HttpStatus responseStatus, Object result) {

        return new ResponseEntity<>(result, responseStatus);
    }
}
