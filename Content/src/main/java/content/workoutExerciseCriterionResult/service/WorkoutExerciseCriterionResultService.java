package content.workoutExerciseCriterionResult.service;

import content.workoutExercise.entity.WorkoutExercise;
import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionResult;
import content.workoutExerciseCriterionResult.repository.WorkoutExerciseCriterionResultRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class WorkoutExerciseCriterionResultService {

    private final WorkoutExerciseCriterionResultRepository workoutExerciseCriterionResultRepository;


    @Autowired
    public WorkoutExerciseCriterionResultService(WorkoutExerciseCriterionResultRepository workoutExerciseCriterionResultRepository) {

        Assert.notNull(workoutExerciseCriterionResultRepository, "workoutExerciseCriterionResultRepository must not be null");

        this.workoutExerciseCriterionResultRepository = workoutExerciseCriterionResultRepository;
    }


    public void saveDefaultResult(UUID userId, WorkoutExercise workoutExercise, UUID criterionId, int value) {

        saveResult(userId, workoutExercise, criterionId, value, null);
    }

    public void saveResult(UUID userId, WorkoutExercise workoutExercise, UUID criterionId, int value, UUID performedWorkoutId) {

        WorkoutExerciseCriterionResult result = new WorkoutExerciseCriterionResult(

                userId,
                workoutExercise.getId(),
                criterionId,
                value,
                performedWorkoutId
        );

        workoutExerciseCriterionResultRepository.save(result);
    }

    public void updateDefaultResult(UUID userId, UUID workoutExerciseId, UUID criterionId, int value) {

        WorkoutExerciseCriterionResult result = workoutExerciseCriterionResultRepository.getByParams(
                userId,
                workoutExerciseId,
                criterionId
        );

        result.setValue(value);

        workoutExerciseCriterionResultRepository.merge(result);
    }
}
