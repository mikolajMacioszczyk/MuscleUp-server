package content.common.wrappers;

import content.common.errors.ValidationError;
import org.springframework.http.HttpStatus;

import java.util.ArrayList;
import java.util.List;

import static content.common.utils.StringUtils.NEW_LINE;
import static content.common.utils.StringUtils.concatenate;

public class ValidationErrors {

    private static final int FIRST_ERROR = 0;
    private final List<ValidationError> errors;


    public ValidationErrors() {

        this.errors = new ArrayList<>();
    }


    public void addError(ValidationError error) {

        errors.add(error);
    }

    public boolean hasErrors() {

        return !errors.isEmpty();
    }

    public void clear() {

        errors.clear();
    }

    public HttpStatus getFirstErrorCode() {

        return errors.get(FIRST_ERROR).response();
    }

    public String getAllErrorsDescription() {

        String description = "Errors:" + NEW_LINE;

        for (ValidationError error : errors) {

            description = concatenate(description, error.description(), NEW_LINE);
        }

        return description;
    }
}
