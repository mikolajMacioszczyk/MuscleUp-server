package content.criterion.controller;

import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.criterion.controller.form.CriterionForm;
import content.criterion.repository.CriterionQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

import static content.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.BAD_REQUEST;


@Service
public class CriterionValidator {

    private final CriterionQuery criterionQuery;


    @Autowired
    private CriterionValidator(CriterionQuery criterionQuery) {

        Assert.notNull(criterionQuery, "criterionQuery must not be null");

        this.criterionQuery = criterionQuery;
    }


    void validateBeforeSave(CriterionForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkName(form.name(), errors);
    }

    void validateBeforeActivate(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkCriterionId(id, errors);
    }

    void validateBeforeDeactivate(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkCriterionId(id, errors);
    }


    public void checkCriterionId(UUID id, ValidationErrors errors) {

        if (criterionQuery.findById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Criterion with ID " + id + " does not exist"));
        }
    }

    private void checkName(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Criterion name can not be empty"));
        }
    }
}
