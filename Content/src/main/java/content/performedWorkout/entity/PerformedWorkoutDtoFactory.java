package content.performedWorkout.entity;

public class PerformedWorkoutDtoFactory {

    public PerformedWorkoutDto create(PerformedWorkout performedWorkout) {

        return new PerformedWorkoutDto(
                performedWorkout.getId(),
                performedWorkout.getWorkout().getDescription(),
                performedWorkout.getUserId(),
                performedWorkout.getTime(),
                performedWorkout.getEntryId()
        );
    }
}
