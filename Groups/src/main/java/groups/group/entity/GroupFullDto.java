package groups.group.entity;

public class GroupFullDto {

    private final Long id;
    private final String name;
    private final  Long maxParticipants;


    public GroupFullDto(Long id, String name, Long maxParticipants) {

        this.id = id;
        this.name = name;
        this.maxParticipants = maxParticipants;
    }


    public Long getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public Long getMaxParticipants() {
        return maxParticipants;
    }
}
