package groups.schedule.service;

import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.service.GroupWorkoutService;
import groups.schedule.controller.form.ScheduleCellForm;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.util.Assert;

import java.util.UUID;

@Service
public class ScheduleEditService {

    private final ScheduleSaver scheduleSaver;
    private final ScheduleUpdater scheduleUpdater;
    private final GroupWorkoutService groupWorkoutService;
    private final GroupWorkoutQuery groupWorkoutQuery;


    @Autowired
    public ScheduleEditService(ScheduleSaver scheduleSaver,
                               ScheduleUpdater scheduleUpdater,
                               GroupWorkoutService groupWorkoutService,
                               GroupWorkoutQuery groupWorkoutQuery) {

        Assert.notNull(scheduleSaver, "scheduleSaver must not be null");
        Assert.notNull(scheduleUpdater, "scheduleUpdater must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");
        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");

        this.scheduleSaver = scheduleSaver;
        this.scheduleUpdater = scheduleUpdater;
        this.groupWorkoutService = groupWorkoutService;
        this.groupWorkoutQuery = groupWorkoutQuery;
    }

    public UUID save(ScheduleCellForm scheduleCellForm) {

        return scheduleSaver.save(scheduleCellForm);
    }

    public UUID saveAsCopy(UUID id, GroupWorkoutForm groupWorkoutForm) {

        UUID cloneId = groupWorkoutQuery.getCloneIdById(id);

        return groupWorkoutService.saveGroupWorkout(groupWorkoutForm, cloneId);
    }

    public UUID singleUpdate(UUID id, ScheduleCellForm scheduleCellForm) {

        return scheduleUpdater.update(id, scheduleCellForm, false);
    }

    public UUID cascadeUpdate(UUID id, ScheduleCellForm scheduleCellForm) {

        return scheduleUpdater.update(id, scheduleCellForm, true);
    }

    public void delete(UUID id) {

        groupWorkoutService.deleteGroupWorkout(id);
    }
}
