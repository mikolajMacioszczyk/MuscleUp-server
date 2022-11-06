package content.workout.controller;

import content.common.abstracts.AbstractListController;
import content.workout.entity.WorkoutDto;
import content.workout.repository.WorkoutQuery;
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
@RequestMapping("workout")
public class WorkoutListController extends AbstractListController {

    private final WorkoutQuery workoutQuery;


    @Autowired
    private WorkoutListController(WorkoutQuery workoutQuery) {

        Assert.notNull(workoutQuery, "workoutQuery must not be null");

        this.workoutQuery = workoutQuery;
    }


    @GetMapping("/{id}")
    protected ResponseEntity<?> getWorkoutById(@PathVariable("id") UUID id) {

        Optional<WorkoutDto> workoutDto = workoutQuery.findById(id);

        return workoutDto.isPresent() ? response(OK, workoutDto.get()) : response(NOT_FOUND);
    }

    @GetMapping("/all")
    protected ResponseEntity<?> getAllWorkouts() {

        List<WorkoutDto> workouts = workoutQuery.getAllWorkouts();

        return response(OK, workouts);
    }
}