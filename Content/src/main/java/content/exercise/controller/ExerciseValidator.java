package content.exercise.controller;

import content.common.errors.ValidationError;
import content.common.wrappers.ValidationErrors;
import content.criterion.controller.CriterionValidator;
import content.exercise.controller.form.ExerciseForm;
import content.exercise.repository.ExerciseQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.List;
import java.util.UUID;

import static content.common.utils.StringUtils.isNullOrEmpty;
import static org.springframework.http.HttpStatus.BAD_REQUEST;

@Service
public class ExerciseValidator {

    private final ExerciseQuery exerciseQuery;
    private final CriterionValidator criterionValidator;


    @Autowired
    private ExerciseValidator(ExerciseQuery exerciseQuery, CriterionValidator criterionValidator) {

        Assert.notNull(exerciseQuery, "exerciseQuery must not be null");
        Assert.notNull(criterionValidator, "criterionValidator must not be null");

        this.exerciseQuery = exerciseQuery;
        this.criterionValidator = criterionValidator;
    }


    void validateBeforeSave(ExerciseForm form, ValidationErrors errors) {

        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkFields(form, errors);
    }

    void validateBeforeUpdate(UUID id, ExerciseForm form, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(form, "form must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkExerciseId(id, errors);
        checkFields(form, errors);
    }

    void validateBeforeDelete(UUID id, ValidationErrors errors) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(errors, "errors must not be null");

        checkExerciseId(id, errors);
    }

    void checkFields(ExerciseForm form, ValidationErrors errors) {

        checkName(form.name(), errors);
        checkDescription(form.description(), errors);
        checkCriteria(form.criteria(), errors);
    }


    private void checkName(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Exercise name can not be empty"));
        }
    }

    private void checkDescription(String name, ValidationErrors errors) {

        if (isNullOrEmpty(name)) {

            errors.addError(new ValidationError(BAD_REQUEST, "Exercise description can not be empty"));
        }
    }

    private void checkExerciseId(UUID id, ValidationErrors errors) {

        if (exerciseQuery.findById(id).isEmpty()) {

            errors.addError(new ValidationError(BAD_REQUEST, "Exercise with given ID does not exist"));
        }
    }

    private void checkCriteria(List<UUID> criteria, ValidationErrors errors) {

        criteria.forEach(id -> criterionValidator.checkCriterionId(id, errors));
    }
}


