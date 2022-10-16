package groups.workoutParticipant.controller;

import groups.workoutParticipant.service.WorkoutParticipantService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("group-workout-participant")
class WorkoutParticipantController {

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
    protected ResponseEntity<UUID> assignToGroupWorkout(@RequestBody WorkoutParticipantForm workoutParticipantForm) {

        return workoutParticipantValidator.isCorrectToAssign(workoutParticipantForm)?
                new ResponseEntity<>(workoutParticipantService.assign(workoutParticipantForm), HttpStatus.OK) :
                new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @PostMapping("/unassign")
    protected ResponseEntity<HttpStatus> deleteGroup(@RequestBody WorkoutParticipantForm workoutParticipantForm) {

        if (workoutParticipantValidator.isCorrectToUnassign(workoutParticipantForm)) {

            workoutParticipantService.unassign(workoutParticipantForm);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @DeleteMapping("/unassign/{id}")
    protected ResponseEntity<HttpStatus> deleteGroup(@PathVariable("id") UUID id) {

        if (workoutParticipantValidator.isCorrectToUnassign(id)) {

            workoutParticipantService.unassign(id);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}
