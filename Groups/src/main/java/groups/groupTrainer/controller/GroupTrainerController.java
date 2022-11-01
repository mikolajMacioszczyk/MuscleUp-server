package groups.groupTrainer.controller;

import groups.common.abstracts.AbstractEditController;
import groups.groupTrainer.controller.form.GroupTrainerForm;
import groups.groupTrainer.service.GroupTrainerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group-trainer")
class GroupTrainerController extends AbstractEditController {

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
    protected ResponseEntity<?> assignTrainerToGroup(@RequestBody GroupTrainerForm groupTrainerForm) {

        groupTrainerValidator.validateBeforeAssign(groupTrainerForm, errors);

        return hasErrors()? errors() : response(OK, groupTrainerService.assign(groupTrainerForm));
    }

    @DeleteMapping("/unassign/{trainerId}/{groupId}")
    protected ResponseEntity<?> unassign(@PathVariable("trainerId") UUID trainerId,
                                         @PathVariable("groupId") UUID groupId) {

        groupTrainerValidator.validateBeforeUnassign(trainerId, groupId, errors);

        if (hasErrors()) return errors();

        groupTrainerService.unassign(trainerId, groupId);

        return response(OK);
    }

    @DeleteMapping("/unassign/{id}")
    protected ResponseEntity<?> unassign(@PathVariable("id") UUID id) {

        groupTrainerValidator.validateBeforeUnassign(id, errors);

        if (hasErrors()) return errors();

        groupTrainerService.unassign(id);

        return response(OK);
    }
}
