package content.common.abstracts;

import content.common.wrappers.ValidationErrors;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RestController;

@RestController
public abstract class AbstractEditController {

    protected ValidationErrors errors;


    protected AbstractEditController() {

        errors = new ValidationErrors();
    }


    protected boolean hasErrors() {

        return errors.hasErrors();
    }

    protected ResponseEntity<?> errors() {

        HttpStatus errorStatus = errors.getFirstErrorCode();
        String description = errors.getAllErrorsDescription();
        errors.clear();

        return new ResponseEntity<>(description, errorStatus);
    }

    protected ResponseEntity<?> response(HttpStatus responseStatus) {

        return new ResponseEntity<>(responseStatus);
    }

    protected ResponseEntity<?> response(HttpStatus responseStatus, Object result) {

        return new ResponseEntity<>(result, responseStatus);
    }
}
