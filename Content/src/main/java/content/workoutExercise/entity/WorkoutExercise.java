package content.workoutExercise.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.exercise.entity.Exercise;
import content.workout.entity.Workout;
import content.workoutExerciseCriterionResult.entity.WorkoutExerciseCriterionResult;
import org.hibernate.annotations.GenericGenerator;

import javax.persistence.*;
import java.util.List;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;
import static javax.persistence.CascadeType.ALL;
import static javax.persistence.FetchType.LAZY;

@Entity
@Table(name = "workout_exercise")
public class WorkoutExercise extends AbstractEntity implements Comparable<WorkoutExercise> {

    @Id
    @Column(name = "workout_exercise_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "sequence_number")
    private int sequenceNumber;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "workout_id")
    private Workout workout;

    @ManyToOne(fetch = FetchType.EAGER)
    @JoinColumn(name = "exercise_id")
    private Exercise exercise;


    @MustExist(reason = HIBERNATE)
    public WorkoutExercise() {
    }

    public WorkoutExercise(int sequenceNumber, Workout workout, Exercise exercise) {

        this.sequenceNumber = sequenceNumber;
        this.workout = workout;
        this.exercise = exercise;
    }


    @Override
    public UUID getId() {
        return id;
    }

    public Exercise getExercise() {
        return exercise;
    }

    public int getSequenceNumber() {
        return sequenceNumber;
    }

    public void setWorkout(Workout workout) {
        this.workout = workout;
    }

    public void setExercise(Exercise exercise) {
        this.exercise = exercise;
    }

    @Override
    public int compareTo(WorkoutExercise other) {

        return Integer.compare(sequenceNumber, other.getSequenceNumber());
    }
}
