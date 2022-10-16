package groups.workout.controller;

import groups.workout.entity.GroupWorkoutFullDto;
import groups.workout.service.GroupWorkoutService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("group-workout")
public class GroupWorkoutEditController {

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
    protected ResponseEntity<UUID> saveGroupWorkout(@RequestBody GroupWorkoutFullForm groupWorkoutFullForm) {

        return groupWorkoutValidator.isCorrectToSave(groupWorkoutFullForm)?
                new ResponseEntity<>(groupWorkoutService.saveGroupWorkout(groupWorkoutFullForm), HttpStatus.OK) :
                new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @PutMapping("/update")
    protected ResponseEntity<UUID> updateGroupWorkout(@RequestBody GroupWorkoutFullDto groupWorkoutFullDto) {

        return groupWorkoutValidator.isCorrectToUpdate(groupWorkoutFullDto)?
                new ResponseEntity<>(groupWorkoutService.updateGroupWorkout(groupWorkoutFullDto), HttpStatus.OK) :
                new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @DeleteMapping("/delete/{id}")
    protected ResponseEntity<HttpStatus> deleteGroupWorkout(@PathVariable("id") UUID id) {

        if (groupWorkoutValidator.isCorrectToDelete(id)) {

            groupWorkoutService.deleteGroupWorkout(id);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}
