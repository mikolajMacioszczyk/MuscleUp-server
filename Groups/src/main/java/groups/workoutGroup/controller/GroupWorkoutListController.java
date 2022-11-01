package groups.workoutGroup.controller;

import groups.common.abstracts.AbstractListController;
import groups.workoutGroup.entity.GroupWorkoutFullDto;
import groups.workoutGroup.repository.GroupWorkoutQuery;
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


    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/full-group-workout-info")
    protected ResponseEntity<?> getAllGroupWorkouts() {

        List<GroupWorkoutFullDto> groupsWorkouts = groupWorkoutQuery.getAllGroupsWorkouts();

        return response(OK, groupsWorkouts);
    }

    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/find/{id}")
    protected ResponseEntity<?> findGroupWorkoutById(@PathVariable("id") UUID id) {

        Optional<GroupWorkoutFullDto> groupWorkoutFullDto = groupWorkoutQuery.findGroupWorkoutById(id);

        return groupWorkoutFullDto.isPresent() ? response(OK, groupWorkoutFullDto.get()) : response(NOT_FOUND);
    }

    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/find-by-workout/{id}")
    protected ResponseEntity<?> findAllGroupWorkoutsByWorkoutId(@PathVariable("id") UUID id) {

        List<GroupWorkoutFullDto> groupsWorkouts = groupWorkoutQuery.getAllGroupWorkoutByWorkoutId(id);

        return response(OK, groupsWorkouts);
    }

    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/find-by-group/{id}")
    protected ResponseEntity<?> findAllGroupWorkoutsByGroupId(@PathVariable("id") UUID id) {

        List<GroupWorkoutFullDto> groupsWorkouts = groupWorkoutQuery.getAllGroupWorkoutByGroupId(id);

        return response(OK, groupsWorkouts);
    }
}
