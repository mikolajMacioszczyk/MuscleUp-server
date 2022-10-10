package groups.group.controller;

import groups.group.entity.GroupFullDto;
import groups.group.service.GroupService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.UUID;

@RestController
@RequestMapping("group")
class GroupEditController {

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
    protected ResponseEntity<UUID> saveGroup(@RequestBody GroupForm groupForm) {

        return groupValidator.isCorrectToSave(groupForm)?
            new ResponseEntity<>(groupService.saveGroup(groupForm), HttpStatus.OK) :
            new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @PutMapping("/update")
    protected ResponseEntity<UUID> updateGroup(@RequestBody GroupFullDto groupFullDto) {

        return groupValidator.isCorrectToUpdate(groupFullDto)?
                new ResponseEntity<>(groupService.updateGroup(groupFullDto), HttpStatus.OK) :
                new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }

    @DeleteMapping("/delete/{id}")
    protected ResponseEntity<HttpStatus> deleteGroup(@PathVariable("id") UUID id) {

        if (groupValidator.isCorrectToDelete(id)) {

            groupService.deleteGroup(id);
            return new ResponseEntity<>(HttpStatus.OK);
        }

        return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
    }
}
