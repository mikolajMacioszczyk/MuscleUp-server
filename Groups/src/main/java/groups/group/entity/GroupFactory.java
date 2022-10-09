package groups.group.entity;

public class GroupFactory {

    public Group create(GroupFullDto groupFullDto) {

        return new Group(
                groupFullDto.getId(),
                groupFullDto.getName(),
                groupFullDto.getMaxParticipants()
        );
    }
}
