package groups.scheduler;

import groups.groupWorkout.controller.form.GroupWorkoutForm;
import groups.groupWorkout.entity.GroupWorkoutDto;
import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.service.GroupWorkoutService;
import org.quartz.*;
import org.springframework.util.Assert;

import java.util.List;
import java.util.logging.Logger;

import static groups.common.utils.EnvironmentUtils.getFutureCreations;
import static java.util.logging.Level.WARNING;

public class JobSignUpFutureGroupWorkouts implements Job {

    private static final int FUTURE_CREATIONS = getFutureCreations();
    private static final Logger logger = Logger.getGlobal();

    @Override
    public void execute(JobExecutionContext context) {

        Assert.notNull(context, "context must not be null");

        try {

            SchedulerContext schedulerContext = context.getScheduler().getContext();

            GroupWorkoutQuery groupWorkoutQuery = (GroupWorkoutQuery) schedulerContext.get("groupWorkoutQuery");
            GroupWorkoutService groupWorkoutService = (GroupWorkoutService) schedulerContext.get("groupWorkoutService");

            performJob(groupWorkoutQuery, groupWorkoutService);
        }
        catch (SchedulerException e) {

            logger.log(WARNING, "Unable to sign up future GroupWorkouts - job execution exception");
        }
    }

    private void performJob(GroupWorkoutQuery groupWorkoutQuery, GroupWorkoutService groupWorkoutService) {

        List<GroupWorkoutDto> todayRepeatableGroupWorkouts = groupWorkoutQuery.getAllRepeatableGroupWorkoutsDayAhead();

        todayRepeatableGroupWorkouts.forEach( toCopy -> {

            GroupWorkoutForm groupWorkoutForm = new GroupWorkoutForm(
                    toCopy.groupId(),
                    toCopy.workoutId(),
                    toCopy.startTime().plusWeeks(FUTURE_CREATIONS),
                    toCopy.endTime().plusWeeks(FUTURE_CREATIONS)
            );

            groupWorkoutService.saveGroupWorkout(groupWorkoutForm, toCopy.cloneId());
        });
    }
}
