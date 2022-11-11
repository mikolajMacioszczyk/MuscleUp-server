package content.workoutExercise.entity;

import content.common.abstracts.AbstractEntity;
import content.common.annotation.MustExist;
import content.exercise.entity.Exercise;
import content.workout.entity.Workout;
import org.hibernate.annotations.GenericGenerator;

import javax.persistence.*;
import java.util.UUID;

import static content.common.annotation.MustExist.Reason.HIBERNATE;

@Entity
@Table(name = "workout_exercise")
public class WorkoutExercise extends AbstractEntity {

    @Id
    @Column(name = "workout_exercise_id")
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(name = "UUID", strategy = "org.hibernate.id.UUIDGenerator")
    private UUID id;

    @Column(name = "sequence_number")
    private int sequenceNumber;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "workout_id")
    private Workout workout;

    @ManyToOne(fetch = FetchType.LAZY)
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
    protected UUID getId() {
        return id;
    }

    public int getSequenceNumber() {
        return sequenceNumber;
    }
}
