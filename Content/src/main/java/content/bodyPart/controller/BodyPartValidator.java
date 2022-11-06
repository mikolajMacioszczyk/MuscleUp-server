package content.bodyPart.controller;

import content.bodyPart.controller.form.BodyPartForm;
import content.bodyPart.repository.BodyPartQuery;
import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static content.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class BodyPartValidator {

    private final BodyPartQuery bodyPartQuery;


    @Autowired
    private BodyPartValidator(BodyPartQuery bodyPartQuery) {

        Assert.notNull(bodyPartQuery, "bodyPartQuery must not be null");

        this.bodyPartQuery = bodyPartQuery;
    }


    void validateBeforeSave(BodyPartForm bodyPartForm, ValidationErrors errors) {

        Assert.notNull(bodyPartForm, "bodyPartForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkName(bodyPartForm.name(), errors);
    }

    void validateBeforeUpdate(UUID id, BodyPartForm bodyPartForm, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(bodyPartForm, "bodyPartForm must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkBodyPartId(id, errors);
        checkName(bodyPartForm.name(), errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkBodyPartId(id, errors);
    }


    private void checkName(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "BodyPart name can not be empty"));
        }
    }

    private void checkBodyPartId(UUID id, ValidationErrors errors) {

        if (bodyPartQuery.findById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "BodyPart with given ID does not exist"));
        }
    }
}
