package content.performedWorkout.controller;

import content.common.abstracts.AbstractEditController;
import content.performedWorkout.controller.form.PerformedWorkoutForm;
import content.performedWorkout.service.PerformedWorkoutService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("performed-workout")
class PerformedWorkoutEditController extends AbstractEditController {

    private final PerformedWorkoutService performedWorkoutService;
    private final PerformedWorkoutValidator performedWorkoutValidator;


    @Autowired
    PerformedWorkoutEditController(PerformedWorkoutService performedWorkoutService, PerformedWorkoutValidator performedWorkoutValidator) {

        Assert.notNull(performedWorkoutService, "performedWorkoutService must not be null");
        Assert.notNull(performedWorkoutValidator, "performedWorkoutValidator must not be null");

        this.performedWorkoutService = performedWorkoutService;
        this.performedWorkoutValidator = performedWorkoutValidator;
    }


    @PostMapping
    protected ResponseEntity<?> savePerformedWorkout(@RequestHeader(FITNESS_CLUB_HEADER) UUID fitnessClubId,
                                                     @RequestBody PerformedWorkoutForm performedWorkoutForm) {

        performedWorkoutValidator.validateBeforeSave(fitnessClubId, performedWorkoutForm, errors);

        return hasErrors()? errors() : response(OK, performedWorkoutService.savePerformedWorkout(performedWorkoutForm));
    }
}