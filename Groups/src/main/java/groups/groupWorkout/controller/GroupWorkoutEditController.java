package groups.groupWorkout.controller;

import groups.common.abstracts.AbstractEditController;
import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.service.GroupWorkoutService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group-workout")
public class GroupWorkoutEditController extends AbstractEditController {

    private final GroupWorkoutService groupWorkoutService;
    private final GroupWorkoutValidator groupWorkoutValidator;


    @Autowired
    private GroupWorkoutEditController(GroupWorkoutService groupWorkoutService, GroupWorkoutValidator groupWorkoutValidator) {

        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");
        Assert.notNull(groupWorkoutValidator, "groupWorkoutValidator must not be null");

        this.groupWorkoutService = groupWorkoutService;
        this.groupWorkoutValidator = groupWorkoutValidator;
    }


    @PostMapping("/save")
    protected ResponseEntity<?> saveGroupWorkout(@RequestBody GroupWorkoutForm groupWorkoutForm) {

        groupWorkoutValidator.validateBeforeSave(groupWorkoutForm, errors);

        return hasErrors()? errors() : response(OK, groupWorkoutService.saveGroupWorkout(groupWorkoutForm));
    }

    @PutMapping("/update/{id}")
    protected ResponseEntity<?> updateGroupWorkout(@PathVariable("id") UUID id, @RequestBody GroupWorkoutForm groupWorkoutForm) {

        groupWorkoutValidator.validateBeforeUpdate(id, groupWorkoutForm, errors);

        return hasErrors()? errors() : response(OK, groupWorkoutService.updateGroupWorkout(id, groupWorkoutForm));
    }

    @DeleteMapping("/delete/{id}")
    protected ResponseEntity<?> deleteGroupWorkout(@PathVariable("id") UUID id) {

        groupWorkoutValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        groupWorkoutService.deleteGroupWorkout(id);

        return response(OK);
    }
}
