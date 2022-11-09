package groups.scheduler;

import groups.groupWorkout.repository.GroupWorkoutQuery;
import groups.groupWorkout.service.GroupWorkoutService;
import org.quartz.*;
import org.quartz.impl.StdSchedulerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;
import org.springframework.util.Assert;

import java.util.logging.Logger;

import static java.util.logging.Level.WARNING;

@Component
public class QuartzConfiguration {

    private static final Logger logger = Logger.getGlobal();

    private Scheduler scheduler;
    private final GroupWorkoutQuery groupWorkoutQuery;
    private final GroupWorkoutService groupWorkoutService;


    @Autowired
    public QuartzConfiguration(GroupWorkoutQuery groupWorkoutQuery, GroupWorkoutService groupWorkoutService) {

        Assert.notNull(groupWorkoutQuery, "groupWorkoutQuery must not be null");
        Assert.notNull(groupWorkoutService, "groupWorkoutService must not be null");

        this.groupWorkoutQuery = groupWorkoutQuery;
        this.groupWorkoutService = groupWorkoutService;

        createScheduler();
        scheduleFutureGroupWorkouts();
    }


    private void createScheduler() {

        try {

            SchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = schedulerFactory.getScheduler();
            scheduler.start();
        }
        catch (SchedulerException e) {

            logger.log(WARNING, "Unable to create scheduler");
        }
    }

    private void scheduleFutureGroupWorkouts() {

        String jobName = "Sign up future GroupWorkouts";

        JobDetail jobDetail = JobBuilder.newJob(JobSignUpFutureGroupWorkouts.class)
                .withIdentity(jobName)
                .build();

        CronTrigger trigger = TriggerBuilder.newTrigger()
                .withIdentity("Every work-day at 3:00AM trigger")
                .withSchedule(CronScheduleBuilder.cronSchedule("0 0 3 ? * 2-6"))
                .forJob(jobName)
                .build();

        try {

            scheduler.getContext().put("groupWorkoutQuery", groupWorkoutQuery);
            scheduler.getContext().put("groupWorkoutService", groupWorkoutService);

            scheduler.scheduleJob(jobDetail, trigger);
        }
        catch (SchedulerException e) {

            logger.log(WARNING, "Unable to schedule job: " + jobName);
        }
    }
}
