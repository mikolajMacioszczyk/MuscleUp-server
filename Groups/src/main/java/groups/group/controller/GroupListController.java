package groups.group.controller;

import groups.common.abstracts.AbstractListController;
import groups.group.entity.GroupDto;
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


    @GetMapping("/{id}")
    protected ResponseEntity<?> getGroupById(@PathVariable("id") UUID id) {

        Optional<GroupDto> groupFullDto = groupQuery.findGroupById(id);

        return groupFullDto.isPresent() ? response(OK, groupFullDto.get()) : response(NOT_FOUND);
    }

    // TODO dla danego klubu
    @GetMapping("/all")
    protected ResponseEntity<?> getAllGroups() {

        List<GroupDto> groups = groupQuery.getAllGroups();

        return response(OK, groups);
    }
}
