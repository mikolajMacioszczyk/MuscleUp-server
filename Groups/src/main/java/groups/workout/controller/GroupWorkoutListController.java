package groups.workout.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

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


    @GetMapping("/find/{id}")
    protected String findAllGroupWorkoutsByWorkoutId(@PathVariable("id") UUID id) {

    }

    @GetMapping("/full-group-workout-info")
    protected String getAllGroupWorkouts() {

    }

    @PostMapping("/find-between-dates")
    protected String findAllGroupWorkoutsByTimePeriod(@RequestBody TimePeriod timePeriod) {

    }

    @PostMapping("/find-between-dates/{id}")
    protected String findAllGroupWorkoutsByWorkoutIdAndTimePeriod(@PathVariable("id") UUID id, @RequestBody TimePeriod timePeriod) {

    }
}
