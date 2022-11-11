package content.exercise.service;

import content.exercise.controller.form.ExerciseForm;
import content.exercise.entity.Exercise;
import content.exercise.entity.ExerciseFactory;
import content.exercise.repository.ExerciseRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class ExerciseService {

    private final ExerciseRepository exerciseRepository;
    private final ExerciseFactory exerciseFactory;


    @Autowired
    public ExerciseService(ExerciseRepository exerciseRepository) {

        Assert.notNull(exerciseRepository, "exerciseRepository must not be null");

        this.exerciseRepository = exerciseRepository;
        this.exerciseFactory = new ExerciseFactory();
    }


    public UUID saveExercise(ExerciseForm form) {

        Assert.notNull(form, "form must not be null");

        Exercise exercise = exerciseFactory.create(form);

        return exerciseRepository.save(exercise);
    }

    public void deleteExercise(UUID id) {

        Assert.notNull(id, "id must not be null");

        exerciseRepository.delete(id);
    }
}
