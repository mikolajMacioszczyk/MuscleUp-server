package groups.schedule.dto;


import groups.group.entity.Group;
import groups.group.trainer.Trainer;
import org.springframework.util.Assert;

public class ScheduleTrainerDtoFactory {

    public ScheduleTrainerDto createEmpty(Group group) {

        Assert.notNull(group, "group must not be null");

        return new ScheduleTrainerDto(
                group.getTrainerId(),
                "",
                "",
                ""
        );
    }

    public ScheduleTrainerDto create(Trainer trainer) {

        Assert.notNull(trainer, "trainer must not be null");

        return new ScheduleTrainerDto(
                trainer.trainerId(),
                trainer.name(),
                trainer.surname(),
                trainer.avatarUrl()
        );
    }
}
