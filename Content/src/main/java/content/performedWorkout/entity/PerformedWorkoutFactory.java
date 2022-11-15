package content.performedWorkout.entity;

import content.performedWorkout.controller.form.PerformedWorkoutForm;
import content.workout.repository.WorkoutRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class PerformedWorkoutFactory {

    private final WorkoutRepository workoutRepository;


    @Autowired
    public PerformedWorkoutFactory(WorkoutRepository workoutRepository) {

        this.workoutRepository = workoutRepository;
    }


    public PerformedWorkout create(PerformedWorkoutForm form) {

        return new PerformedWorkout(
                workoutRepository.getById(form.workoutId()),
                form.userId(),
                form.time()
        );
    }
}
