package groups.group.controller;

import groups.common.abstracts.AbstractListController;
import groups.group.entity.GroupFullDto;
import groups.group.entity.GroupNameDto;
import groups.group.repository.GroupQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static org.springframework.http.HttpStatus.NOT_FOUND;
import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group")
public class GroupListController extends AbstractListController {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupListController(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    @GetMapping("/find/{id}")
    protected ResponseEntity<?> findGroupById(@PathVariable("id") UUID id) {

        Optional<GroupFullDto> groupFullDto = groupQuery.findGroupById(id);

        return groupFullDto.isPresent() ? response(OK, groupFullDto.get()) : response(NOT_FOUND);
    }

    // TODO wszystkie grupy, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/full-group-info")
    protected ResponseEntity<?> getAllGroups() {

        List<GroupFullDto> groups = groupQuery.getAllGroups();

        return response(OK, groups);
    }

    // TODO wszystkie grupy, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/group-names")
    protected ResponseEntity<?> getGroupNames() {

        List<GroupNameDto> groups = groupQuery.getAllGroupNames();

        return response(OK, groups);
    }
}
