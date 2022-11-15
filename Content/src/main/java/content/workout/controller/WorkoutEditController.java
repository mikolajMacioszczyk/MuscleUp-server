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


    @PostMapping
    protected ResponseEntity<?> saveWorkout(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId,
                                            @RequestBody WorkoutForm form) {

        workoutValidator.validateBeforeSave(fitnessClubId, form, errors);

        return hasErrors()? errors() : response(OK, workoutService.saveWorkout(fitnessClubId, form));
    }

    @PostMapping("/{id}")
    protected ResponseEntity<?> updateWorkout(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId,
                                              @PathVariable("id") UUID id,
                                              @RequestBody WorkoutForm form) {

        workoutValidator.validateBeforeUpdate(id, fitnessClubId, form, errors);

        return hasErrors()? errors() : response(OK, workoutService.updateWorkout(id, fitnessClubId, form));
    }

    @DeleteMapping("/{id}")
    protected ResponseEntity<?> deleteWorkout(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId,
                                              @PathVariable("id") UUID id) {

        workoutValidator.validateBeforeDelete(id, fitnessClubId, errors);

        if (hasErrors()) return errors();

        workoutService.deleteWorkout(id);

        return response(OK);
    }
}