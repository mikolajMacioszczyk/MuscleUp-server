package groups.group.entity;

public class GroupNameDto {

    private final Long id;
    private final String name;


    public GroupNameDto(Long id, String name) {

        this.id = id;
        this.name = name;
    }


    public Long getId() {
        return id;
    }

    public String getName() {
        return name;
    }
}
