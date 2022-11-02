package groups.workoutPermission.controller;

import groups.common.abstracts.AbstractEditController;
import groups.workoutPermission.controller.form.GroupPermissionForm;
import groups.workoutPermission.service.GroupPermissionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group-permission")
class GroupPermissionController extends AbstractEditController {

    private final GroupPermissionService groupPermissionService;
    private final GroupPermissionValidator groupPermissionValidator;


    @Autowired
    private GroupPermissionController(GroupPermissionService groupPermissionService,
                                      GroupPermissionValidator groupPermissionValidator) {

        Assert.notNull(groupPermissionService, "groupPermissionService must not be null");
        Assert.notNull(groupPermissionValidator, "groupPermissionValidator must not be null");

        this.groupPermissionService = groupPermissionService;
        this.groupPermissionValidator = groupPermissionValidator;
    }


    @PostMapping("/add")
    protected ResponseEntity<?> addGroupPermission(@RequestBody GroupPermissionForm groupPermissionForm) {

        groupPermissionValidator.validateBeforeAdd(groupPermissionForm, errors);

        return hasErrors()? errors() : response(OK, groupPermissionService.add(groupPermissionForm));
    }

    @DeleteMapping("/remove/{groupId}/{permissionId}")
    protected ResponseEntity<?> removeGroupPermission(@PathVariable("groupId") UUID groupId,
                                                      @PathVariable("permissionId") UUID permissionId) {

        groupPermissionValidator.validateBeforeRemove(groupId, permissionId, errors);

        if (hasErrors()) return errors();

        groupPermissionService.remove(groupId, permissionId);

        return response(OK);
    }

    @DeleteMapping("/remove/{id}")
    protected ResponseEntity<?> removeGroupPermission(@PathVariable("id") UUID id) {

        groupPermissionValidator.validateBeforeRemove(id, errors);

        if (hasErrors()) return errors();

        groupPermissionService.remove(id);

        return response(OK);
    }
}
