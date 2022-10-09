package groups.group.controller;

import groups.group.entity.GroupFullDto;
import groups.group.entity.GroupNameDto;
import groups.group.repository.GroupQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("group")
public class GroupListController {

    private final GroupQuery groupQuery;


    @Autowired
    private GroupListController(GroupQuery groupQuery) {

        Assert.notNull(groupQuery, "groupQuery must not be null");

        this.groupQuery = groupQuery;
    }


    @GetMapping("/find-by-id/{id}")
    protected ResponseEntity<GroupFullDto> findGroupById(@PathVariable("id") Long id) {

        Optional<GroupFullDto> groupFullDto = groupQuery.findGroupById(id);

        return groupFullDto.map(fullDto -> new ResponseEntity<>(fullDto, HttpStatus.FOUND))
                .orElseGet(() -> new ResponseEntity<>(HttpStatus.NOT_FOUND));
    }

    @GetMapping("/full-group-info")
    protected ResponseEntity<List<GroupFullDto>> getAllGroups() {

        List<GroupFullDto> groups = groupQuery.getAllGroups();

        return new ResponseEntity<>(groups, HttpStatus.OK);
    }

    @GetMapping("/group-names")
    protected ResponseEntity<List<GroupNameDto>> getGroupNames() {

        List<GroupNameDto> groups = groupQuery.getAllGroupNames();

        return new ResponseEntity<>(groups, HttpStatus.OK);
    }
}
