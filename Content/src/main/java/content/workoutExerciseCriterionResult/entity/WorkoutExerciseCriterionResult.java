package content.workoutExerciseCriterionResult.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.criterion.entity.Criterion;
import content.workoutExercise.entity.WorkoutExercise;
import org.hibernate.annotations.GenericGenerator;
import org.springframework.lang.Nullable;
import org.springframework.util.Assert;

import javax.persistence.*;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;

@Entity
@Table(name = "workout_exercise_criterion_result")
public class WorkoutExerciseCriterionResult extends AbstractEntity {

    @Id
    @Column(name = "workout_exercise_criterion_result_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "user_id")
    private UUID userId;

    @Column(name = "workout_exercise_id")
    private UUID workoutExerciseId;

    @Column(name = "criterion_id")
    private UUID criterionId;

    @Column(name = "value")
    private int value;

    @Column(name = "performed_workout_id")
    private UUID performedWorkoutId;


    @MustExist(reason = HIBERNATE)
    public WorkoutExerciseCriterionResult() {
    }

    public WorkoutExerciseCriterionResult(UUID userId,
                                          UUID workoutExerciseId,
                                          UUID criterionId,
                                          int value,
                                          @Nullable UUID performedWorkoutId) {

        Assert.notNull(userId, "userId must not be null");
        Assert.notNull(workoutExerciseId, "workoutExerciseId must not be null");
        Assert.notNull(criterionId, "criterionId must not be null");

        this.userId = userId;
        this.workoutExerciseId = workoutExerciseId;
        this.criterionId = criterionId;
        this.value = value;
        this.performedWorkoutId = performedWorkoutId;
    }

    @MustExist(reason = HIBERNATE)
    public WorkoutExerciseCriterionResult(UUID id,
                                          UUID userId,
                                          UUID workoutExerciseId,
                                          UUID criterionId,
                                          int value,
                                          @Nullable UUID performedWorkoutId) {

        Assert.notNull(id, "id must not be null");
        Assert.notNull(userId, "userId must not be null");
        Assert.notNull(workoutExerciseId, "workoutExerciseId must not be null");
        Assert.notNull(criterionId, "criterionId must not be null");

        this.id = id;
        this.userId = userId;
        this.workoutExerciseId = workoutExerciseId;
        this.criterionId = criterionId;
        this.value = value;
        this.performedWorkoutId = performedWorkoutId;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public UUID getUserId() {
        return userId;
    }

    public UUID getCriterionId() {
        return criterionId;
    }

    public UUID getWorkoutExerciseId() {
        return workoutExerciseId;
    }

    public int getValue() {
        return value;
    }

    public void setValue(int value) {
        this.value = value;
    }

    public UUID getPerformedWorkoutId() {
        return performedWorkoutId;
    }
}