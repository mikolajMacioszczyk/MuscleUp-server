package groups.groupPermission.controller;

import groups.common.abstracts.AbstractEditController;
import groups.groupPermission.controller.form.GroupPermissionForm;
import groups.groupPermission.service.GroupPermissionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

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


    @PostMapping("/assign")
    protected ResponseEntity<?> assignGroupPermission(@RequestBody GroupPermissionForm groupPermissionForm) {

        groupPermissionValidator.validateBeforeAssign(groupPermissionForm, errors);

        return hasErrors()? errors() : response(OK, groupPermissionService.assign(groupPermissionForm));
    }

    @PutMapping("/unassign")
    protected ResponseEntity<?> unassignGroupPermission(@RequestBody GroupPermissionForm groupPermissionForm) {

        groupPermissionValidator.validateBeforeUnassign(groupPermissionForm, errors);

        if (hasErrors()) return errors();

        groupPermissionService.unassign(groupPermissionForm);

        return response(OK);
    }
}
