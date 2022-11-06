package groups.group.controller;

import groups.common.abstracts.AbstractEditController;
import groups.group.controller.form.GroupForm;
import groups.group.service.GroupService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;


@RestController
@RequestMapping("group")
class GroupEditController extends AbstractEditController {

    private final GroupService groupService;
    private final GroupValidator groupValidator;


    @Autowired
    private GroupEditController(GroupService groupService, GroupValidator groupValidator) {

        Assert.notNull(groupService, "groupService must not be null");
        Assert.notNull(groupValidator, "groupValidator must not be null");

        this.groupService = groupService;
        this.groupValidator = groupValidator;
    }


    @PostMapping("/save")
    protected ResponseEntity<?> saveGroup(@RequestBody GroupForm groupForm) {

        groupValidator.validateBeforeSave(groupForm, errors);

        return hasErrors()? errors() : response(OK, groupService.saveGroup(groupForm));
    }

    @PutMapping("/update/{id}")
    protected ResponseEntity<?> updateGroup(@PathVariable("id") UUID id, @RequestBody GroupForm groupForm) {

        groupValidator.validateBeforeUpdate(id, groupForm, errors);

        return hasErrors()? errors() : response(OK, groupService.updateGroup(id, groupForm));
    }

    @DeleteMapping("/delete/{id}")
    protected ResponseEntity<?> deleteGroup(@PathVariable("id") UUID id) {

        groupValidator.validateBeforeDelete(id, errors);

        if (hasErrors()) return errors();

        groupService.deleteGroup(id);

        return response(OK);
    }
}
