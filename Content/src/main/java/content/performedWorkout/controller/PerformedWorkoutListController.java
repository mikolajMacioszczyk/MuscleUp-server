package content.performedWorkout.controller;

import content.common.abstracts.AbstractListController;
import content.performedWorkout.entity.PerformedWorkoutDto;
import content.performedWorkout.repository.PerformedWorkoutQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.UUID;

import static org.springframework.http.HttpStatus.OK;

@RestController
@RequestMapping("performed-workout")
public class PerformedWorkoutListController extends AbstractListController {

    private final PerformedWorkoutQuery performedWorkoutQuery;


    @Autowired
    private PerformedWorkoutListController(PerformedWorkoutQuery performedWorkoutQuery) {

        Assert.notNull(performedWorkoutQuery, "performedWorkoutQuery must not be null");

        this.performedWorkoutQuery = performedWorkoutQuery;
    }


    @GetMapping("/all")
    protected ResponseEntity<?> getAllPerformedWorkouts() {

        List<PerformedWorkoutDto> performedWorkouts = performedWorkoutQuery.getAllPerformedWorkouts();

        return response(OK, performedWorkouts);
    }


    @GetMapping("/performer/{userId}")
    protected ResponseEntity<?> getAllPerformedWorkoutsByUserId(@PathVariable("userId") UUID userId) {

        List<PerformedWorkoutDto> performedWorkouts = performedWorkoutQuery.getAllPerformedWorkoutsByUserId(userId);

        return response(OK, performedWorkouts);
    }

    @GetMapping("/creator/{creatorId}")
    protected ResponseEntity<?> getAllPerformedWorkoutsByCreatorId(@PathVariable("creatorId") UUID creatorId) {

        List<PerformedWorkoutDto> performedWorkouts = performedWorkoutQuery.getAllPerformedWorkoutsByCreatorId(creatorId);

        return response(OK, performedWorkouts);
    }
}
