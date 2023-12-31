package groups.groupWorkout.controller;

import groups.common.abstracts.AbstractListController;
import groups.groupWorkout.entity.GroupWorkoutDto;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

import static org.springframework.http.HttpStatus.NOT_FOUND;
import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("group-workout")
public class GroupWorkoutListController extends AbstractListController {

    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    private GroupWorkoutListController(GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
    }


    // TODO danego klubu
    @GetMapping("/{id}")
    protected ResponseEntity<?> findGroupWorkoutById(@PathVariable("id") UUID id) {

        Optional<GroupWorkoutDto> groupWorkoutFullDto = groupWorkoutQuery.findGroupWorkoutById(id);

        return groupWorkoutFullDto.isPresent() ? response(OK, groupWorkoutFullDto.get()) : response(NOT_FOUND);
    }

    // TODO danego klubu
    @GetMapping("/all")
    protected ResponseEntity<?> getAllGroupWorkouts() {

        List<GroupWorkoutDto> groupsWorkouts = groupWorkoutQuery.getAllGroupsWorkouts();

        return response(OK, groupsWorkouts);
    }
}
