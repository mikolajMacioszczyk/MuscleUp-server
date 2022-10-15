package groups.workout.controller;

import groups.workout.entity.GroupWorkoutFullDto;
import groups.workout.repository.GroupWorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@RestController
@RequestMapping("class-workout")
public class GroupWorkoutListController {

    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    private GroupWorkoutListController(GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
    }


    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/full-class-workout-info")
    protected ResponseEntity<List<GroupWorkoutFullDto>> getAllGroupWorkouts() {

        List<GroupWorkoutFullDto> groupsWorkouts = groupWorkoutQuery.getAllGroupsWorkouts();

        return new ResponseEntity<>(groupsWorkouts, HttpStatus.OK);
    }

    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/find/{id}")
    protected ResponseEntity<GroupWorkoutFullDto> findGroupWorkoutById(@PathVariable("id") UUID id) {

        Optional<GroupWorkoutFullDto> groupWorkoutFullDto = groupWorkoutQuery.findGroupWorkoutById(id);

        return groupWorkoutFullDto.map(fullDto -> new ResponseEntity<>(fullDto, HttpStatus.FOUND))
                .orElseGet(() -> new ResponseEntity<>(HttpStatus.NOT_FOUND));
    }

    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/find-by-workout/{id}")
    protected ResponseEntity<List<GroupWorkoutFullDto>> findAllGroupWorkoutsByWorkoutId(@PathVariable("id") UUID id) {

        List<GroupWorkoutFullDto> groupsWorkouts = groupWorkoutQuery.getAllGroupWorkoutByWorkoutId(id);

        return new ResponseEntity<>(groupsWorkouts, HttpStatus.OK);
    }

    // TODO wszystkie, ale dla danego klubu - pobieramy z tokenu
    @GetMapping("/find-by-class/{id}")
    protected ResponseEntity<List<GroupWorkoutFullDto>> findAllGroupWorkoutsByGroupId(@PathVariable("id") UUID id) {

        List<GroupWorkoutFullDto> groupsWorkouts = groupWorkoutQuery.getAllGroupWorkoutByGroupId(id);

        return new ResponseEntity<>(groupsWorkouts, HttpStatus.OK);
    }
}
