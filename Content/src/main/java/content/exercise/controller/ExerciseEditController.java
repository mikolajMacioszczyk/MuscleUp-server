package content.exercise.controller;

import content.common.abstracts.AbstractEditController;
import content.exercise.controller.form.ExerciseForm;
import content.exercise.service.ExerciseService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("exercise")
class ExerciseEditController extends AbstractEditController {

    private final ExerciseService exerciseService;
    private final ExerciseValidator exerciseValidator;


    @Autowired
    ExerciseEditController(ExerciseService exerciseService, ExerciseValidator exerciseValidator) {

        Assert.notNull(exerciseService, "exerciseService must not be null");
        Assert.notNull(exerciseValidator, "exerciseValidator must not be null");

        this.exerciseService = exerciseService;
        this.exerciseValidator = exerciseValidator;
    }


    @PostMapping
    protected ResponseEntity<?> saveExercise(@RequestBody ExerciseForm exerciseForm) {

        exerciseValidator.validateBeforeSave(exerciseForm, errors);

        return hasErrors()? errors() : response(OK, exerciseService.saveExercise(exerciseForm));
    }

    @PutMapping
    protected ResponseEntity<?> updateExercise(@PathVariable("id") UUID id, @RequestBody ExerciseForm exerciseForm) {

        exerciseValidator.validateBeforeUpdate(id, exerciseForm, errors);

        return hasErrors()? errors() : response(OK, exerciseService.updateExercise(id, exerciseForm));
    }

    @DeleteMapping("/{id}")
    protected ResponseEntity<?> deleteExercise(@PathVariable("id") UUID id) {

        exerciseValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        exerciseService.deleteExercise(id);

        return response(OK);
    }
}