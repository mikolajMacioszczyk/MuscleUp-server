package content.workout.controller;

import content.common.abstracts.AbstractEditController;
import content.workout.controller.form.WorkoutForm;
import content.workout.service.WorkoutService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("workout")
class WorkoutEditController extends AbstractEditController {

    private final WorkoutService workoutService;
    private final WorkoutValidator workoutValidator;


    @Autowired
    WorkoutEditController(WorkoutService workoutService, WorkoutValidator workoutValidator) {

        Assert.notNull(workoutService, "workoutService must not be null");
        Assert.notNull(workoutValidator, "bodyPartValidator must not be null");

        this.workoutService = workoutService;
        this.workoutValidator = workoutValidator;
    }


    @PostMapping("/save")
    protected ResponseEntity<?> saveWorkout(@RequestBody WorkoutForm workoutForm) {

        workoutValidator.validateBeforeSave(workoutForm, errors);

        return hasErrors()? errors() : response(OK, workoutService.saveWorkout(workoutForm));
    }

    @DeleteMapping("/delete/{id}")
    protected ResponseEntity<?> deleteWorkout(@PathVariable("id") UUID id) {

        workoutValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        workoutService.deleteWorkout(id);

        return response(OK);
    }

    @PutMapping("/{workoutId}/add-body-part/{bodyPartId}")
    protected ResponseEntity<?> addBodyPart(@PathVariable("workoutId") UUID workoutId,
                                            @PathVariable("bodyPartId") UUID bodyPartId) {

        workoutValidator.validateBeforeAddBodyPart(workoutId, bodyPartId, errors);

        return hasErrors()? errors() : response(OK, workoutService.addBodyPart(workoutId, bodyPartId));
    }

    @DeleteMapping("/{workoutId}/remove-body-part/{bodyPartId}")
    protected ResponseEntity<?> removeBodyPart(@PathVariable("workoutId") UUID workoutId,
                                               @PathVariable("bodyPartId") UUID bodyPartId) {

        workoutValidator.validateBeforeRemoveBodyPart(workoutId, bodyPartId, errors);

        return hasErrors()? errors() : response(OK, workoutService.removeBodyPart(workoutId, bodyPartId));
    }
}