package groups.workoutParticipant.controller;

import groups.common.abstracts.AbstractEditController;
import groups.workoutParticipant.controller.form.WorkoutParticipantForm;
import groups.workoutParticipant.service.WorkoutParticipantService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group-workout-participant")
class WorkoutParticipantController extends AbstractEditController {

    private final WorkoutParticipantService workoutParticipantService;
    private final WorkoutParticipantValidator workoutParticipantValidator;


    @Autowired
    private WorkoutParticipantController(WorkoutParticipantService workoutParticipantService,
                                         WorkoutParticipantValidator workoutParticipantValidator) {

        Assert.notNull(workoutParticipantService, "workoutParticipantService must not be null");
        Assert.notNull(workoutParticipantValidator, "workoutParticipantValidator must not be null");

        this.workoutParticipantService = workoutParticipantService;
        this.workoutParticipantValidator = workoutParticipantValidator;
    }


    @PostMapping("/assign")
    protected ResponseEntity<?> assignToGroupWorkout(@RequestBody WorkoutParticipantForm workoutParticipantForm) {

        workoutParticipantValidator.validateBeforeAssign(workoutParticipantForm, errors);

        return hasErrors()? errors() : response(OK, workoutParticipantService.assign(workoutParticipantForm));
    }

    @PutMapping("/unassign")
    protected ResponseEntity<?> deleteGroup(@RequestBody WorkoutParticipantForm workoutParticipantForm) {

        workoutParticipantValidator.validateBeforeUnassign(workoutParticipantForm, errors);

        if (hasErrors()) return errors();

        workoutParticipantService.unassign(workoutParticipantForm);

        return response(OK);
    }
}
