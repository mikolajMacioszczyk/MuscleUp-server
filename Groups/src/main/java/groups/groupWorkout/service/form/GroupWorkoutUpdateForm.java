package groups.groupWorkout.service.form;

import groups.common.utils.TimeUtils;

import java.util.UUID;

public record GroupWorkoutUpdateForm(
        UUID groupId,
        UUID workoutId,
        TimeUtils.TimeDiff startTimeDiff,
        TimeUtils.TimeDiff endTimeDiff,
        UUID cloneId
) { }
