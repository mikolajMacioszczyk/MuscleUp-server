package groups.groupWorkout.entity;

import groups.common.abstracts.AbstractEntity;
import groups.common.annotation.MustExist;
import groups.common.annotation.Reason;
import groups.group.entity.Group;

import javax.persistence.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "classWorkout")
public class GroupWorkout extends AbstractEntity {

    @Id
    private String id;

    @Column(nullable = false)
    private LocalDateTime startTime;

    @Column(nullable = false)
    private LocalDateTime endTime;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "group_id")
    private Group group;

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "workout_id")
    private Workout workout;


    @MustExist(reason = Reason.HIBERNATE)
    public GroupWorkout() {
    }


}
