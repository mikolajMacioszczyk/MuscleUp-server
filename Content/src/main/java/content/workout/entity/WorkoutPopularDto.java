package content.workout.entity;


public record WorkoutPopularDto(String workoutName, int performances) implements Comparable<WorkoutPopularDto> {

    @Override
    public int compareTo(WorkoutPopularDto other) {

        return Integer.compare(performances, other.performances());
    }
}
