package groups.groupTrainer.controller;

import groups.groupTrainer.controller.form.GroupTrainerForm;
import groups.groupTrainer.service.GroupTrainerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("group-trainer")
class GroupTrainerController {

    private final GroupTrainerService groupTrainerService;
    private final GroupTrainerValidator groupTrainerValidator;


    @Autowired
    private GroupTrainerController(GroupTrainerService groupTrainerService, GroupTrainerValidator groupTrainerValidator) {

        Assert.notNull(groupTrainerService, "groupTrainerService must not be null");
        Assert.notNull(groupTrainerValidator, "groupTrainerValidator must not be null");

        this.groupTrainerService = groupTrainerService;
        this.groupTrainerValidator = groupTrainerValidator;
    }


    @PostMapping("/assign")
    protected ResponseEntity<UUID> assignTrainerToGroup(@RequestBody GroupTrainerForm groupTrainerForm) {

        return groupTrainerValidator.isCorrectToAssign(groupTrainerForm)?
                new ResponseEntity<>(groupTrainerService.assign(groupTrainerForm), HttpStatus.OK) :
                new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @PostMapping("/unassign")
    protected ResponseEntity<HttpStatus> deleteGroup(@RequestBody GroupTrainerForm groupTrainerForm) {

        if (groupTrainerValidator.isCorrectToUnassign(groupTrainerForm)) {

            groupTrainerService.unassign(groupTrainerForm);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @DeleteMapping("/unassign/{id}")
    protected ResponseEntity<HttpStatus> deleteGroup(@PathVariable("id") UUID id) {

        if (groupTrainerValidator.isCorrectToUnassign(id)) {

            groupTrainerService.unassign(id);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}
