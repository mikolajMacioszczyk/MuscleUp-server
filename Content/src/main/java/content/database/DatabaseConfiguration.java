package content.database;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.env.Environment;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import org.springframework.orm.hibernate5.LocalSessionFactoryBean;
import org.springframework.transaction.annotation.EnableTransactionManagement;
import org.springframework.util.Assert;

import javax.sql.DataSource;
import java.util.Properties;

@Configuration
@EnableTransactionManagement
public class DatabaseConfiguration {

    private static final String DB_URL = "spring.datasource.url";
    private static final String DB_USERNAME = "spring.datasource.username";
    private static final String DB_PASSWORD = "spring.datasource.password";
    private static final String DB_HIBERNATE_DIALECT = "spring.jpa.properties.hibernate.dialect";
    private static final String DB_GENERATE_DDL = "hibernate.hbm2ddl.auto";
    private static final String DB_DRIVER = "spring.datasource.driver-class-name";
    private static final String ENTITY_MANAGER_PACKAGES_TO_SCAN = "entitymanager.packagesToScan";

    private final Environment env;


    @Autowired
    DatabaseConfiguration(Environment env) {

        Assert.notNull(env, "env must not be null");

        this.env = env;
    }


    @Bean
    public LocalSessionFactoryBean sessionFactory() {

        LocalSessionFactoryBean sessionFactory = new LocalSessionFactoryBean();

        sessionFactory.setDataSource(dataSource());
        sessionFactory.setPackagesToScan(env.getProperty(ENTITY_MANAGER_PACKAGES_TO_SCAN));
        sessionFactory.setHibernateProperties(hibernateProperties());

        return sessionFactory;
    }

    private Properties hibernateProperties() {

        Properties hibernateProperties = new Properties();

        hibernateProperties.setProperty(DB_HIBERNATE_DIALECT, env.getProperty(DB_HIBERNATE_DIALECT));
        hibernateProperties.setProperty(DB_GENERATE_DDL, env.getProperty(DB_GENERATE_DDL));

        return hibernateProperties;
    }

    @Bean
    public DataSource dataSource() {

        DriverManagerDataSource dataSource = new DriverManagerDataSource();

        dataSource.setUrl(env.getProperty(DB_URL));
        dataSource.setDriverClassName(env.getProperty(DB_DRIVER));
        dataSource.setUsername(env.getProperty(DB_USERNAME));
        dataSource.setPassword(env.getProperty(DB_PASSWORD));

        return dataSource;
    }
}
