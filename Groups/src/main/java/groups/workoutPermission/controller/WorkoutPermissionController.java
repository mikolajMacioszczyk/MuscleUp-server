package groups.workoutPermission.controller;

import groups.common.abstracts.AbstractEditController;
import groups.workoutPermission.controller.form.WorkoutPermissionForm;
import groups.workoutPermission.service.WorkoutPermissionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group-workout-permission")
class WorkoutPermissionController extends AbstractEditController {

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
    protected ResponseEntity<?> addGroupPermission(@RequestBody WorkoutPermissionForm workoutPermissionForm) {

        workoutPermissionValidator.validateBeforeAdd(workoutPermissionForm, errors);

        return hasErrors()? errors() : response(OK, workoutPermissionService.add(workoutPermissionForm));
    }

    @DeleteMapping("/remove/{groupWorkoutId}/{permissionId}")
    protected ResponseEntity<?> removeGroupPermission(@PathVariable("groupWorkoutId") UUID groupWorkoutId,
                                                           @PathVariable("permissionId") UUID permissionId) {

        workoutPermissionValidator.validateBeforeRemove(groupWorkoutId, permissionId, errors);

        if (hasErrors()) return errors();

        workoutPermissionService.remove(groupWorkoutId, permissionId);

        return response(OK);
    }

    @DeleteMapping("/remove/{id}")
    protected ResponseEntity<?> removeGroupPermission(@PathVariable("id") UUID id) {

        workoutPermissionValidator.validateBeforeRemove(id, errors);

        if (hasErrors()) return errors();

        workoutPermissionService.remove(id);

        return response(OK);
    }
}
