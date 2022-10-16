package groups.workoutPermission.controller;

import groups.workoutPermission.controller.form.WorkoutPermissionForm;
import groups.workoutPermission.service.WorkoutPermissionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("group-workout-permission")
class WorkoutPermissionController {

    private final WorkoutPermissionService workoutPermissionService;
    private final WorkoutPermissionValidator workoutPermissionValidator;


    @Autowired
    private WorkoutPermissionController(WorkoutPermissionService workoutPermissionService,
                                        WorkoutPermissionValidator workoutPermissionValidator) {

        Assert.notNull(workoutPermissionService, "workoutPermissionService must not be null");
        Assert.notNull(workoutPermissionValidator, "workoutPermissionValidator must not be null");

        this.workoutPermissionService = workoutPermissionService;
        this.workoutPermissionValidator = workoutPermissionValidator;
    }


    @PostMapping("/add")
    protected ResponseEntity<UUID> addToGroupPermission(@RequestBody WorkoutPermissionForm workoutPermissionForm) {

        return workoutPermissionValidator.isCorrectToAdd(workoutPermissionForm)?
                new ResponseEntity<>(workoutPermissionService.add(workoutPermissionForm), HttpStatus.OK) :
                new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @PostMapping("/remove")
    protected ResponseEntity<HttpStatus> removePermission(@RequestBody WorkoutPermissionForm workoutPermissionForm) {

        if (workoutPermissionValidator.isCorrectToRemove(workoutPermissionForm)) {

            workoutPermissionService.remove(workoutPermissionForm);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @DeleteMapping("/remove/{id}")
    protected ResponseEntity<HttpStatus> deletePermission(@PathVariable("id") UUID id) {

        if (workoutPermissionValidator.isCorrectToRemove(id)) {

            workoutPermissionService.remove(id);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}
