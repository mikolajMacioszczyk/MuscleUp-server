package content.exercise.service;

import content.exercise.controller.form.ExerciseForm;
import content.exercise.entity.Exercise;
import content.exercise.entity.ExerciseDto;
import content.exercise.entity.ExerciseFactory;
import content.exercise.repository.ExerciseQuery;
import content.exercise.repository.ExerciseRepository;
import content.workoutExercise.repository.WorkoutExerciseQuery;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class ExerciseService {

    private final ExerciseQuery exerciseQuery;
    private final ExerciseRepository exerciseRepository;
    private final WorkoutExerciseQuery workoutExerciseQuery;
    private final ExerciseFactory exerciseFactory;


    @Autowired
    public ExerciseService(ExerciseQuery exerciseQuery,
                           ExerciseRepository exerciseRepository,
                           WorkoutExerciseQuery workoutExerciseQuery,
                           ExerciseFactory exerciseFactory) {

        Assert.notNull(exerciseQuery, "exerciseQuery must not be null");
        Assert.notNull(exerciseRepository, "exerciseRepository must not be null");
        Assert.notNull(workoutExerciseQuery, "workoutExerciseQuery must not be null");
        Assert.notNull(exerciseFactory, "exerciseFactory must not be null");

        this.exerciseQuery = exerciseQuery;
        this.exerciseRepository = exerciseRepository;
        this.workoutExerciseQuery = workoutExerciseQuery;
        this.exerciseFactory = exerciseFactory;
    }


    public UUID saveExercise(ExerciseForm form) {

        Assert.notNull(form, "form must not be null");

        Exercise exercise = exerciseFactory.create(form);

        return exerciseRepository.save(exercise);
    }

    public UUID updateExercise(UUID id, ExerciseForm form) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(form, "form must not be null");

        ExerciseDto exerciseDto = exerciseQuery.get(id);

        if(exerciseDto.isDifferent(form)) {

            expire(id);

            return saveExercise(form);
        }

        return id;
    }

    public void deleteExercise(UUID id) {

        Assert.notNull(id, "id must not be null");

        expire(id);

        if (!workoutExerciseQuery.isExerciseConnected(id)) {

            exerciseRepository.delete(id);
        }
    }

    private void expire(UUID id) {

        Exercise exercise = exerciseRepository.getById(id);

        exercise.setLatest(false);

        exerciseRepository.update(exercise);
    }
}
